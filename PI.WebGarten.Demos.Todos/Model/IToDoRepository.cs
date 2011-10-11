namespace PI.WebGarten.Demos.Todos.Model
{
    using System.Collections.Generic;

    interface IToDoRepository  
    {
        IEnumerable<ToDo> GetAll();
        ToDo GetById(int id);
        void Add(ToDo td);
    }
}