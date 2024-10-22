import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { CreateProfileDto } from '../../models/profile.model';
import { ParticipantsService } from '../../services/participants.service';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'app-registration-modal',
  templateUrl: './registration-modal.component.html',
  styleUrls: ['./registration-modal.component.css']
})
export class RegistrationModalComponent implements OnInit {
  @Input() isModalOpen: boolean = false;
  @Input() isAuthorized: boolean = false;
  @Input() eventId: number | null = null;
  @Output() close = new EventEmitter<void>();

  registrationForm: FormGroup;

  constructor(
    private fb: FormBuilder,
    private participantsService: ParticipantsService,
    private snackBar: MatSnackBar
  ) {
    this.registrationForm = this.fb.group({
      eventId: [null, Validators.required],
      name: ['', Validators.required],
      surname: ['', Validators.required],
      dateOfBirthday: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]]
    });
  }

  ngOnInit(): void {
    if (this.eventId) {
      this.registrationForm.patchValue({ eventId: this.eventId });
    }
  }

  registerForEvent() {
    console.log(this.registrationForm);
    if (this.registrationForm.invalid) {
      console.log('Форма невалидна', this.registrationForm.errors);
      return;
    }

    if (!this.isAuthorized) {
      this.snackBar.open('Пожалуйста, авторизуйтесь для регистрации на событие.', 'Закрыть', {
        duration: 5000,
      });
      return;
    }

    const profile: CreateProfileDto = this.registrationForm.value;

    this.participantsService.registerUser(profile).subscribe(() => {
      console.log('Успешно зарегистрировано на событие');
      this.closeModal();
      this.snackBar.open('Вы успешно зарегистрировались на событие!', 'Закрыть', {
        duration: 5000,
      });
    }, error => {
      console.error('Ошибка при регистрации на событие', error);
      this.snackBar.open('Ошибка при регистрации на событие.', 'Закрыть', {
        duration: 5000,
      });
    });
  }

  closeModal() {
    this.close.emit();
  }
}
