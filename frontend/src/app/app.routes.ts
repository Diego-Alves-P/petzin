import { Routes } from '@angular/router';
import { Canis } from './features/canis/canis';
import { Pets } from './features/pets/pets';
import { Reservas } from './features/reservas/reservas';
import { Tutores } from './features/tutores/tutores';

export const routes: Routes = [
  { path: '', pathMatch: 'full', redirectTo: 'tutores' },
  { path: 'tutores', component: Tutores },
  { path: 'pets', component: Pets },
  { path: 'canis', component: Canis },
  { path: 'reservas', component: Reservas }
];
