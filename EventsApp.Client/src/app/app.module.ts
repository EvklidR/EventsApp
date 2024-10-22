import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { LoginComponent } from './components/login/login.component';
import { RegisterComponent } from './components/register/register.component';
import { EventsComponent } from './components/events/events.component';
import { MyEventsComponent } from './components/my-events/my-events.component';
import { TokenInterceptor } from './services/interceptors/token.interceptor';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { EventDetailsComponent } from './components/event-details/event-details.component';
import { provideAnimationsAsync } from '@angular/platform-browser/animations/async';
import { RegistrationModalComponent } from './components/registration-modal/registration-modal.component';
import { EditEventModalComponent } from './components/edit-event-modal/edit-event-modal.component';
import { CreateEventComponent } from './components/create-event/create-event.component';


@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    RegisterComponent,
    EventsComponent,
    MyEventsComponent,
    EventDetailsComponent,
    RegistrationModalComponent,
    EditEventModalComponent,
    CreateEventComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    ReactiveFormsModule,
    HttpClientModule,
    MatSnackBarModule
  ],
  providers: [
    {
      provide: HTTP_INTERCEPTORS,
      useClass: TokenInterceptor,
      multi: true
    },
    provideAnimationsAsync()
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
