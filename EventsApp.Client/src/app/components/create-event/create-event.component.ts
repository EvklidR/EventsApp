import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { EventsService } from '../../services/events.service';
import { CategoryOfEvent } from '../../models/event.model';
import { Router } from '@angular/router';

@Component({
  selector: 'app-create-event',
  templateUrl: './create-event.component.html',
  styleUrls: ['./create-event.component.css']
})
export class CreateEventComponent implements OnInit {
  eventForm!: FormGroup;
  categories = Object.values(CategoryOfEvent).filter(key => isNaN(Number(key)));

  constructor(
    private fb: FormBuilder,
    private eventsService: EventsService,
    private router: Router
  ) { }

  ngOnInit(): void {
    this.eventForm = this.fb.group({
      name: ['', Validators.required],
      description: ['', Validators.required],
      dateTimeHolding: ['', Validators.required],
      location: ['', Validators.required],
      category: ['', Validators.required],
      maxParticipants: [0, [Validators.required, Validators.min(1)]],
      imageFile: [null, Validators.required]
    });
  }

  onSubmit(): void {
    if (this.eventForm.invalid) {
      return;
    }

    const formData = new FormData();

    Object.keys(this.eventForm.controls).forEach(key => {
      if (key === 'imageFile') {
        const fileInput = this.eventForm.get('imageFile')?.value;
        if (fileInput && fileInput.files && fileInput.files.length > 0) {
          formData.append(key, fileInput.files[0]);
        } else {
          console.error('Файл не выбран');
          return;
        }
      } else {
        formData.append(key, this.eventForm.get(key)?.value);
      }
    });

    this.eventsService.createEvent(formData).subscribe(
      () => {
        this.router.navigate(['/events']);
      },
      error => {
        console.error('Ошибка при создании события', error);
      }
    );
  }

  onFileSelected(event: any): void {
    const file = event.target.files[0];
    if (file) {
      this.eventForm.patchValue({
        imageFile: event.target
      });
    }
  }
}
