import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { BrowserModule } from '@angular/platform-browser';
import { RouterModule } from '@angular/router';

import { AppComponent } from './app.component';

import { HttpClientModule } from '@angular/common/http';
import { SingleImageComponent } from './singleimage/singleimage.component';
import { ImagesComponent } from './images/images.component';

@NgModule({
  declarations: [
    AppComponent,
      SingleImageComponent,
      ImagesComponent
   ],
  imports: [
    BrowserModule,
    FormsModule,
    HttpClientModule,
    RouterModule.forRoot([
      {path:"", redirectTo:"/index", pathMatch:"full"},
      {path:"index", component:ImagesComponent},
      {path:"image/:id", component:SingleImageComponent}
    ])
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
