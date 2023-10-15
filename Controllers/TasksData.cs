using Newtonsoft.Json;

namespace tasksUI
{
    public static class TasksData
    {
        public static List<TaskHolder> StorageTasks { get; private set; } = new();
        public static List<TaskHolder> DbTasks { get; private set; } = new();

        static string StorageKey => "tasks";

        public static async Task Init()
        {
            string tasksJson = await SecureStorage.Default.GetAsync(StorageKey);
            if (tasksJson == null) return;

            StorageTasks = JsonConvert.DeserializeObject<List<TaskHolder>>(tasksJson);
        }

        public static async Task SaveTasks()
        {
            string jsonStr = JsonConvert.SerializeObject(StorageTasks);
            await SecureStorage.Default.SetAsync(StorageKey, jsonStr);
        }
    }
}