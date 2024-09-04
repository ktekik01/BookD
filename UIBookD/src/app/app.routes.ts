// app.routes.ts
import { Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { LoginComponent } from './login/login.component';
import { RegisterComponent } from './register/register.component';
import { DashboardComponent } from './dashboard/dashboard.component';
import { ReviewsPageComponent } from './reviews-page/reviews-page.component';
import { BooksPageComponent } from './books-page/books-page.component';
import { UsersPageComponent } from './users-page/users-page.component';
import { ListsPageComponent } from './lists-page/lists-page.component';
import { ListDetailComponent } from './list-detail/list-detail.component';
import { AskToAIComponent } from './ask-to-ai/ask-to-ai.component';
import { MyListsComponent } from './my-lists/my-lists.component';
import { MyReviewsComponent } from './my-reviews/my-reviews.component';
import { MyProfileComponent } from './my-profile/my-profile.component';
import { ChattingPageComponent } from './chatting-page/chatting-page.component';
import { ChatComponent } from './chat/chat.component';
import { ReviewDetailComponent } from './review-detail/review-detail.component';

export const routes: Routes = [
  { path: 'home', component: HomeComponent },
  { path: 'register', component: RegisterComponent },
  { path: 'login', component: LoginComponent },
  { path: 'dashboard', component: DashboardComponent },
  { path: 'reviews-page', component: ReviewsPageComponent },
  { path: 'books-page', component: BooksPageComponent },
  { path: 'users-page', component: UsersPageComponent},
  { path: 'lists-page', component: ListsPageComponent},
  { path: 'list-detail/:id', component: ListDetailComponent },
  { path: 'ask-to-ai', component: AskToAIComponent },
  { path: 'my-lists', component: MyListsComponent },
  { path: 'my-reviews', component: MyReviewsComponent },
  { path: 'my-profile', component: MyProfileComponent },
  { path: 'chatting-page', component: ChattingPageComponent},
  { path: 'chatting-page/:id', component: ChatComponent},
  { path: 'review-detail/:id', component: ReviewDetailComponent },
  { path: '', redirectTo: '/home', pathMatch: 'full' },
];
