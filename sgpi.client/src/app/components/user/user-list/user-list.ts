import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { RouterLink } from '@angular/router';
import { UserService } from '@services/user';
import { Usuario } from '../../../data/types/usuario';

@Component({
  selector: 'app-user-list',
  standalone: true,
  imports: [CommonModule, MatTableModule, MatButtonModule, MatIconModule, RouterLink],
  templateUrl: './user-list.html',
  styleUrls: ['./user-list.scss'],
})
export class UserListComponent implements OnInit {
  private userService = inject(UserService);
  users: Usuario[] = [];
  displayedColumns: string[] = ['nome', 'email', 'cargo', 'roles', 'actions'];

  ngOnInit(): void {
    this.loadUsers();
  }

  loadUsers(): void {
    this.userService.getAll().subscribe((data) => {
      this.users = data;
    });
  }

  deleteUser(id: string): void {
    if (confirm('Tem certeza que deseja excluir este usuário?')) {
      this.userService.delete(id).subscribe(() => {
        this.loadUsers();
      });
    }
  }
}
