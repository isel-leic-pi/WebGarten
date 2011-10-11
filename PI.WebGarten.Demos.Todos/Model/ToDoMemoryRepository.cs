namespace PI.WebGarten.Demos.Todos.Model
{
    using System.Collections.Generic;

    class ToDoMemoryRepository : IToDoRepository
    {
        private readonly IDictionary<int, ToDo> _repo = new Dictionary<int, ToDo>();
        private int _cid = 0;

        public IEnumerable<ToDo> GetAll()
        {
            return _repo.Values;
        }

        public ToDo GetById(int id)
        {
            ToDo td = null;
            _repo.TryGetValue(id, out td);
            return td;
        }

        public void Add(ToDo td)
        {
            td.Id = _cid;
            _repo.Add(_cid++,td);
        }
    }
}