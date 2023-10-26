using System.Net;
using System.Text;
using Newtonsoft.Json;

namespace tasksUI
{
    public static class TaskService
    {
        public static List<TaskModel> Tasks { get; private set; } = new();

        static string StorageKey => "tasks";

        public static event Action<HttpRequestMessage> onRequestFail;

        static readonly HttpClient httpClient = new()
        {
            BaseAddress = new Uri("https://tasksweb001.azurewebsites.net/"),
        };

        public static async void AddTask(TaskModel task)
        {
            Tasks.Add(task);
            StringContent content = new(JsonConvert.SerializeObject(task), Encoding.UTF8, "application/json");
            using HttpResponseMessage res = await httpClient.PostAsync("api/tasks", content);
            res.EnsureSuccessStatusCode();
        }

        public static async void DeleteTask(TaskModel task)
        {
            Tasks.Remove(task);
            using HttpResponseMessage res = await httpClient.DeleteAsync($"api/tasks/{task.Id}");
        }

        /// <summary>
        /// will load tasks two times, one from cache and other from database
        /// </summary>
        public static async void LoadTasks(Action<List<TaskModel>> onTasksLoad)
        {
            var tcached = await GetCachedTasks();
            Tasks = tcached;
            onTasksLoad?.Invoke(Tasks);

            var tdb = await GetDbTasks();
            if (tdb != null)
                Tasks = CompareTasks(tcached, tdb);
            onTasksLoad?.Invoke(Tasks);
        }

        static List<TaskModel> CompareTasks(List<TaskModel> current, List<TaskModel> incoming)
        {
            var tasksToKeep = current.Where(t => !t.Linked);
            incoming.AddRange(tasksToKeep);
            return incoming;
        }

        static async Task<List<TaskModel>> GetCachedTasks()
        {
            string tasksJson = await SecureStorage.Default.GetAsync(StorageKey);
            if (tasksJson == null) return null;

            return JsonConvert.DeserializeObject<List<TaskModel>>(tasksJson);
        }

        static async Task<List<TaskModel>> GetDbTasks()
        {
            using HttpResponseMessage res = await httpClient.GetAsync("api/tasks");
            if (res.StatusCode != HttpStatusCode.OK)
            {
                onRequestFail?.Invoke(res.RequestMessage);
                return null;
            }

            string tasksJson = await res.Content.ReadAsStringAsync();
            var tasks = JsonConvert.DeserializeObject<List<TaskModel>>(tasksJson);
            foreach (TaskModel t in tasks)
                t.Linked = true;

            return tasks;
        }

        public static async Task SaveTasks(List<TaskModel> tasks)
        {
            string jsonStr = JsonConvert.SerializeObject(tasks);
            await SecureStorage.Default.SetAsync(StorageKey, jsonStr);
        }

        public static void UpdateTask(TaskModel th) => th.GetVisualModel().BackgroundColor = th.Done ? new Color(128, 0, 128) : null;
    }
}