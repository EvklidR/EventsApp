import { Component, OnInit } from '@angular/core';
import { ParticipantsService } from '../../services/participants.service';
import { EventsService } from '../../services/events.service';
import { Event } from '../../models/event.model';

@Component({
  selector: 'app-my-events',
  templateUrl: './my-events.component.html',
  styleUrls: ['./my-events.component.css']
})
export class MyEventsComponent implements OnInit {
  myEvents: Event[] = [];

  constructor(
    private participantsService: ParticipantsService,
    private eventsService: EventsService
  ) { }

  ngOnInit(): void {
    this.loadUserEvents();
  }

  loadUserEvents() {
    this.participantsService.getUserEvents().subscribe(
      (events: Event[]) => {
        this.myEvents = events;
      },
      (error) => {
        console.error('Ошибка при загрузке событий пользователя', error);
      }
    );
  }

  unregisterFromEvent(eventId: number) {
    this.participantsService.unregisterUser(eventId).subscribe(
      () => {
        this.myEvents = this.myEvents.filter(event => event.id !== eventId);
        console.log('Регистрация на событие отменена');
      },
      (error) => {
        console.error('Ошибка при отмене регистрации на событие', error);
      }
    );
  }
}
