import { Component, OnInit, ChangeDetectionStrategy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Task, TaskService } from './task.service';
import { RouterModule, NavigationEnd, Router } from '@angular/router';
import { filter } from 'rxjs/operators';

@Component({
  selector: 'app-task',
  standalone: true,
  templateUrl: './task.component.html',
  styleUrls: ['./task.component.css'],
  imports: [CommonModule, FormsModule, RouterModule],
  changeDetection: ChangeDetectionStrategy.Default,
  providers: [TaskService]
})
export class TaskComponent implements OnInit {
  tasks: Task[] = []; // Lista de tarefas
  editingTask: Task | null = null; // Tarefa em edição
  newTaskTitle: string = ''; // Título para nova tarefa

  constructor(private taskService: TaskService, private router: Router) {}

  ngOnInit(): void {
    // Carrega as tarefas quando o componente é inicializado
    this.loadTasks();

    // Recarrega as tarefas sempre que a rota muda
    this.router.events
      .pipe(filter((event) => event instanceof NavigationEnd))
      .subscribe(() => {
        this.loadTasks();
      });
  }

  private loadTasks(): void {
    this.taskService.getTasks().subscribe({
      next: (tasks) => {
        this.tasks = tasks || [];
      },

    });
  }

  // Excluir uma tarefa
  deleteTask(id: string): void {
    this.taskService.deleteTask(id).subscribe({
      next: () => {
        this.loadTasks(); // Recarrega a lista após excluir
      },
      error: (err) => {
        console.error('Error deleting task:', err);
      }
    });
  }

  // Editar uma tarefa
  editTask(task: Task): void {
    this.editingTask = { ...task }; // Cria uma cópia da tarefa para edição
  }

  // Salvar alterações na tarefa
  saveTask(): void {
    if (this.editingTask) {
      this.taskService.updateTask(this.editingTask).subscribe({
        next: () => {
          this.editingTask = null; // Finaliza o estado de edição
          this.loadTasks(); // Recarrega a lista
        },
        error: (err) => {
          console.error('Error updating task:', err);
        }
      });
    }
  }

  // Cancelar edição
  cancelEdit(): void {
    this.editingTask = null;
  }

  trackByTaskId(index: number, task: Task): string {
    return task.id || index.toString();
  }
}
