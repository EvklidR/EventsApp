import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { EventsService } from '../../services/events.service';
import { AuthService } from '../../services/auth.service';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Event, CategoryOfEvent } from '../../models/event.model';

@Component({
  selector: 'app-event-details',
  templateUrl: './event-details.component.html',
  styleUrls: ['./event-details.component.css']
})
export class EventDetailsComponent implements OnInit {
  event: Event | null = null;
  imageUrl: string | null = null;
  imageNotFound: boolean = false;
  isModalOpen: boolean = false;
  isEditModalOpen: boolean = false;
  isAuthorized: boolean = false;

  constructor(
    private route: ActivatedRoute,
    private eventsService: EventsService,
    private authService: AuthService,
    private snackBar: MatSnackBar,
    private router: Router
  ) { }

  ngOnInit(): void {
    this.authService.isAuthorized$.subscribe(isAuthorized => {
      this.isAuthorized = isAuthorized;
    });

    this.loadEvent();
  }

  loadEvent(): void {
    const eventId = this.route.snapshot.paramMap.get('id');
    if (eventId) {
      this.eventsService.getEventById(Number(eventId)).subscribe(
        (event: Event) => {
          this.event = event;
          if (event.imageUrl) {
            this.loadImage(event.imageUrl);
          }
        },
        (error) => {
          console.error('Ошибка при получении информации о событии', error);
        }
      );
    }
  }

  loadImage(fileName: string): void {
    this.eventsService.getEventImage(fileName).subscribe(
      (imageBlob: Blob) => {
        const reader = new FileReader();
        reader.onload = () => {
          this.imageUrl = reader.result as string;
        };
        reader.readAsDataURL(imageBlob);
      },
      (error) => {
        console.error('Ошибка при загрузке изображения', error);
        this.imageNotFound = true;
      }
    );
  }

  openModal() {
    this.isModalOpen = true;
  }

  closeModal() {
    this.isModalOpen = false;
  }

  openEditModal() {
    this.isEditModalOpen = true;
  }

  closeEditModal() {
    this.isEditModalOpen = false;
  }

  updateEvent(updatedEvent: Event) {
    this.event = updatedEvent;
    this.snackBar.open('Событие успешно обновлено!', 'Закрыть', {
      duration: 3000,
    });
    this.closeEditModal();
  }

  get eventCategory(): string {
    return this.event ? CategoryOfEvent[this.event.category] || 'Неизвестная категория' : 'Неизвестная категория';
  }

  deleteEvent(eventId: number): void {
    console.log(`Удалить событие с ID: ${eventId}`);
    this.eventsService.deleteEvent(eventId).subscribe(() => {
      this.snackBar.open('Событие успешно удалено', 'Закрыть', {
        duration: 3000
      });
      this.router.navigate(['/events']);
    }, error => {
      console.error('Ошибка при удалении события', error);
      this.snackBar.open('Ошибка при удалении события', 'Закрыть', {
        duration: 3000
      });
    });
  }

  isAdmin(): boolean {
    return this.authService.isAdmin();
  }
}
