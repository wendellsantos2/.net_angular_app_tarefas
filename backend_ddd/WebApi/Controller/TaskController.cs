using Entities.Entidades;
using Infra.repository;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace webApi.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class TaskController : ControllerBase
    {
        private readonly RepositoryBase<TaskEntity> _taskRepository;

        public TaskController(RepositoryBase<TaskEntity> taskRepository)
        {
            _taskRepository = taskRepository;
        }

        /// <summary>
        /// Obtém uma tarefa pelo ID.
        /// </summary>
        /// <param name="id">ID da tarefa</param>
        /// <returns>TaskEntity</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var task = await _taskRepository.GetByIdAsync(id);
            if (task == null)
                return NotFound(new { Message = "Task not found" });

            return Ok(task);
        }

        /// <summary>
        /// Obtém todas as tarefas.
        /// </summary>
        /// <returns>Lista de TaskEntity</returns>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var tasks = await _taskRepository.ListAsync();

            if (!tasks.Any())
                return NoContent();

            return Ok(tasks);
        }

        /// <summary>
        /// Cria uma nova tarefa.
        /// </summary>
        /// <param name="task">Objeto da tarefa</param>
        /// <returns>Tarefa criada</returns>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] TaskEntity task)
        {
            if (task == null || string.IsNullOrWhiteSpace(task.Name))
                return BadRequest(new { Message = "Invalid task data" });

            task.Id = Guid.NewGuid();
            task.CreatedAt = DateTime.UtcNow;

            await _taskRepository.AddAsync(task);

            return CreatedAtAction(nameof(GetById), new { id = task.Id }, task);
        }

        /// <summary>
        /// Atualiza uma tarefa existente.
        /// </summary>
        /// <param name="id">ID da tarefa</param>
        /// <param name="task">Objeto da tarefa</param>
        /// <returns>NoContent se sucesso</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] TaskEntity task)
        {
            if (id != task.Id)
                return BadRequest(new { Message = "Task ID mismatch" });

            var existingTask = await _taskRepository.GetByIdAsync(id);
            if (existingTask == null)
                return NotFound(new { Message = "Task not found" });

            task.UpdatedAt = DateTime.UtcNow;

            await _taskRepository.UpdateAsync(task);

            return NoContent();
        }

        /// <summary>
        /// Deleta uma tarefa pelo ID.
        /// </summary>
        /// <param name="id">ID da tarefa</param>
        /// <returns>NoContent se sucesso</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var existingTask = await _taskRepository.GetByIdAsync(id);
            if (existingTask == null)
                return NotFound(new { Message = "Task not found" });

            await _taskRepository.DeleteAsync(id);

            return Ok(new { Message = "Task deleted successfully" });
        }
    }
}
