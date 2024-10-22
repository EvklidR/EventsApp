import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { CreateProfileDto, ParticipantOfEventDto } from '../models/profile.model';
import { Event } from '../models/event.model';

@Injectable({
  providedIn: 'root'
})
export class ParticipantsService {
  private apiUrl = 'https://localhost:7068/events/api/Participants';

  constructor(private http: HttpClient) { }

  registerUser(profile: CreateProfileDto): Observable<void> {
    return this.http.post<void>(this.apiUrl, profile);
  }

  unregisterUser(eventId: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${eventId}`);
  }

  getUserEvents(): Observable<Event[]> {
    return this.http.get<Event[]>(`${this.apiUrl}/user`);
  }

  getParticipantsByEventId(eventId: number): Observable<ParticipantOfEventDto[]> {
    return this.http.get<ParticipantOfEventDto[]>(`${this.apiUrl}/event/${eventId}`);
  }
}
