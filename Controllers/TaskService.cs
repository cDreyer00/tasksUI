using System.Net;
using Newtonsoft.Json;

namespace tasksUI
{
    public static class TaskService
    {
        public static List<TaskModel> StorageTasks { get; private set; } = new();
        public static List<TaskModel> DbTasks { get; private set; } = new();

        static string StorageKey => "tasks";

        public static event Action<HttpRequestMessage> onRequestFail;

        private static HttpClient httpClient = new()
        {
            BaseAddress = new Uri("https://tasksweb001.azurewebsites.net/"),
        };

        public static async Task<List<TaskModel>> GetCachedTasks()
        {
             string tasksJson = await SecureStorage.Default.GetAsync(StorageKey);
            if (tasksJson == null) return null;

            return JsonConvert.DeserializeObject<List<TaskModel>>(tasksJson);
        }

        public static async Task<List<TaskModel>> GetDbTasks()
        {
            using HttpResponseMessage res = await httpClient.GetAsync("api/tasks");
            if (res.StatusCode != HttpStatusCode.OK)
            {
                onRequestFail?.Invoke(res.RequestMessage);
                return null;
            }

            string tasksJson = await res.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<TaskModel>>(tasksJson);
        }

        public static async Task SaveTasks(List<TaskModel> tasks)
        {
            string jsonStr = JsonConvert.SerializeObject(tasks);
            await SecureStorage.Default.SetAsync(StorageKey, jsonStr);
        }
    }
}