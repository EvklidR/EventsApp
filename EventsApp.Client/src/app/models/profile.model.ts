export interface CreateProfileDto {
  userId?: number;
  eventId: number;
  name: string;
  surname: string;
  dateOfBirthday: string;
  email: string;
}

export interface ParticipantOfEventDto {
  id: number;
  eventId: number;
  name: string;
  surname: string;
  email: string;
  dateOfBirthday: string;
  dateOfRegistration: string;
}
