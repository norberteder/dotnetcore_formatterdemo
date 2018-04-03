import { User } from './user';

export class Appointment {
  organizer: User;
  location: string;
  title: string;
  description: string;
  start: Date;
  end: Date;
}
