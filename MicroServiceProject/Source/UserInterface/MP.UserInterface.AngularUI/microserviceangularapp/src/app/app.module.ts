import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { AddcustomerComponent } from './addcustomer/addcustomer.component';
import { UpdatecustomerComponent } from './components/updatecustomer/updatecustomer.component';
import { RemovecustomerComponent } from './components/removecustomer/removecustomer.component';
import { ListcustomerComponent } from './components/listcustomer/listcustomer.component';

@NgModule({
  declarations: [
    AppComponent,
    AddcustomerComponent,
    UpdatecustomerComponent,
    RemovecustomerComponent,
    ListcustomerComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
