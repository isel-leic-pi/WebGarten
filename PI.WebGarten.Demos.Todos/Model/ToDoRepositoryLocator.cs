namespace PI.WebGarten.Demos.Todos.Model
{
    class ToDoRepositoryLocator
    {
        private readonly static IToDoRepository Repo = new ToDoMemoryRepository();
        public static IToDoRepository Get()
        {
            return Repo;
        }
    }
}