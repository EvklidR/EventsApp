import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Event, CreateEventDto, EventFilterDto } from '../models/event.model';

@Injectable({
  providedIn: 'root'
})
export class EventsService {
  private apiUrl = 'https://localhost:7068/events/Events';

  constructor(private http: HttpClient) { }

  getEvents(eventFilterDto: EventFilterDto): Observable<Event[]> {
    let params = new HttpParams();

    if (eventFilterDto.Date) {
      params = params.set('Date', eventFilterDto.Date.toISOString());
    }
    if (eventFilterDto.Location) {
      params = params.set('Location', eventFilterDto.Location);
    }
    if (eventFilterDto.Category) {
      params = params.set('Category', eventFilterDto.Category);
    }
    if (eventFilterDto.PageNumber) {
      params = params.set('PageNumber', eventFilterDto.PageNumber.toString());
    }
    if (eventFilterDto.PageSize) {
      params = params.set('PageSize', eventFilterDto.PageSize.toString());
    }

    return this.http.get<Event[]>(this.apiUrl, { params });
  }

  getEventById(id: number): Observable<Event> {
    return this.http.get<Event>(`${this.apiUrl}/${id}`);
  }

  getUserEvents(): Observable<Event[]> {
    return this.http.get<Event[]>(`${this.apiUrl}/user-events`);
  }

  getEventImage(fileName: string): Observable<Blob> {
    return this.http.get(`${this.apiUrl}/get-file/${fileName}`, { responseType: 'blob' });
  }

  createEvent(eventData: FormData): Observable<Event> {
    return this.http.post<Event>(this.apiUrl, eventData);
  }

  updateEvent(event: Event): Observable<void> {
    return this.http.put<void>(this.apiUrl, event);
  }

  deleteEvent(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}
