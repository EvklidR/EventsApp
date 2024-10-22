import { Component, Input, Output, EventEmitter } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { UpdateEventDto, Event } from '../../models/event.model'; // Импортируем оба типа
import { EventsService } from '../../services/events.service';
import { MatSnackBar } from '@angular/material/snack-bar';
import { CategoryOfEvent } from '../../models/event.model'; // Обновите путь к вашему файлу, если нужно

@Component({
  selector: 'app-edit-event-modal',
  templateUrl: './edit-event-modal.component.html',
  styleUrls: ['./edit-event-modal.component.css']
})
export class EditEventModalComponent {
  @Input() isModalOpen: boolean = false;
  @Input() event: Event | null = null;
  @Output() close = new EventEmitter<Event>();
  editEventForm: FormGroup;
  CategoryOfEvent = CategoryOfEvent;

  constructor(private fb: FormBuilder, private eventsService: EventsService, private snackBar: MatSnackBar) {
    this.editEventForm = this.fb.group({
      name: ['', Validators.required],
      description: ['', Validators.required],
      dateTimeHolding: ['', Validators.required],
      location: ['', Validators.required],
      category: ['', Validators.required],
      maxParticipants: ['', [Validators.required, Validators.min(1)]]
    });
  }

  ngOnChanges(): void {
    if (this.event) {
      this.editEventForm.patchValue({
        name: this.event.name,
        description: this.event.description,
        dateTimeHolding: this.event.dateTimeHolding,
        location: this.event.location,
        category: this.event.category,
        maxParticipants: this.event.maxParticipants
      });
    }
  }

  submit() {
    if (this.editEventForm.valid && this.event) {
      const updatedEvent: Event = {
        ...this.event,
        ...this.editEventForm.value,
      };

      this.eventsService.updateEvent(updatedEvent).subscribe(
        () => {
          console.log('Событие успешно обновлено', updatedEvent);
          this.snackBar.open('Событие успешно обновлено!', 'Закрыть', {
            duration: 3000,
          });

          this.close.emit(updatedEvent);
        },
        (error) => {
          console.error('Ошибка при обновлении события', error);
          this.snackBar.open('Ошибка при обновлении события', 'Закрыть', {
            duration: 3000,
          });
        }
      );
    }
  }

  closeModal() {
    this.close.emit(this.event!);
    this.editEventForm.reset();
  }
}
