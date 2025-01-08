import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { HttpClientModule } from '@angular/common/http';
import { AppComponent } from './app.component';

@NgModule({
  declarations: [

    // Outros componentes
  ],
  imports: [
    BrowserModule,
    HttpClientModule
    // Outros módulos
  ],
  providers: [],
  bootstrap: [ ]
})
export class AppModule { }
