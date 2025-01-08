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
  changeDetection: ChangeDetectionStrategy.Default, // Usa Default para garantir a atualização automática
  providers: [TaskService]
})
export class TaskComponent implements OnInit {
  tasks: Task[] = []; // Lista de tarefas

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
      error: (err) => {
        console.error('Error loading tasks:', err);
      }
    });
  }

  trackByTaskId(index: number, task: Task): string {
    return task.id || index.toString();
  }

  
}
