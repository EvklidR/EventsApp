import { ParticipantOfEventDto } from './profile.model';

export enum CategoryOfEvent {
  Educational,
  Entertainment,
  Sports,
  Social,
  Thematic
}

export interface Event {
  id: number;
  name: string;
  description: string;
  dateTimeHolding: Date;
  location: string;
  category: CategoryOfEvent;
  maxParticipants: number;
  imageUrl?: string;
  participants: ParticipantOfEventDto[];
}

export interface CreateEventDto {
  name: string;
  description: string;
  dateTimeHolding: Date;
  location: string;
  category: CategoryOfEvent;
  maxParticipants: number;
}

export interface UpdateEventDto {
  id: number;
  name: string;
  description: string;
  dateTimeHolding: Date;
  location: string;
  category: CategoryOfEvent;
  maxParticipants: number;
}

export interface EventFilterDto {
  Date: Date | null;
  Location: string;
  Category: CategoryOfEvent | null;
  PageNumber: number;
  PageSize: number;
}
