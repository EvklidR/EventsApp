import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { EventsService } from '../../services/events.service';
import { Event } from '../../models/event.model';
import { AuthService } from '../../services/auth.service';
import { MatSnackBar } from '@angular/material/snack-bar';
import { EventFilterDto } from '../../models/event.model';

@Component({
  selector: 'app-events',
  templateUrl: './events.component.html',
  styleUrls: ['./events.component.css']
})
export class EventsComponent implements OnInit {
  events: Event[] = [];
  isAuthorized: boolean = false;
  isAdmin: boolean = false;
  filterForm: FormGroup;

  categories = [
    'Educational',
    'Entertainment',
    'Sports',
    'Social',
    'Thematic'
  ];

  constructor(
    private eventsService: EventsService,
    private authService: AuthService,
    private snackBar: MatSnackBar,
    private fb: FormBuilder
  ) {

    this.filterForm = this.fb.group({
      Date: [null],
      Location: [''],
      Category: [null],
      PageNumber: [1],
      PageSize: [10] 
    });
  }

  ngOnInit(): void {
    this.loadEvents();
    this.authService.isAuthorized$.subscribe(isAuthorized => {
      this.isAuthorized = isAuthorized;
      this.isAdmin = this.authService.isAdmin();
    });
  }

  loadEvents() {
    const filterDto: EventFilterDto = this.filterForm.value;
    this.eventsService.getEvents(filterDto).subscribe(data => {
      this.events = data;
    });
  }

  applyFilters() {
    this.loadEvents();
  }

  deleteEvent(eventId: number) {
    this.eventsService.deleteEvent(eventId).subscribe(() => {
      this.loadEvents();
      this.snackBar.open('Событие удалено', 'Закрыть', { duration: 3000 });
    });
  }
}
