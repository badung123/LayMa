import { Component, OnDestroy, OnInit } from '@angular/core';
import { IconDirective } from '@coreui/icons-angular';
import { ContainerComponent, 
  RowComponent, 
  ColComponent, 
  TextColorDirective, 
  CardComponent, 
  CardBodyComponent, 
  FormDirective, 
  InputGroupComponent, 
  InputGroupTextDirective, 
  FormControlDirective, 
  ButtonDirective, 
  CardHeaderComponent, 
  TableDirective, 
  FormFloatingDirective, 
  FormLabelDirective, 
  FormSelectDirective,
  DropdownComponent,
  DropdownItemDirective,
  DropdownMenuDirective,
  DropdownToggleDirective,
 } from '@coreui/angular';
import { Subject, takeUntil } from 'rxjs';
import { AlertService } from '../../shared/services/alert.service';
import { AdminApiUserApiClient,ProcessStatus, UserDtoInList, UserDtoInListPagedResult, VerifyOrLockUserRequest } from '../../api/admin-api.service.generated';
import { CommonModule, NgStyle } from '@angular/common';
import {
  FormBuilder,
  FormControl,
  FormGroup,
  FormsModule,
  NgModel,
  ReactiveFormsModule,
  Validators
} from '@angular/forms';
import { Router } from '@angular/router';
import { environment } from '../../../environments/environment';
import { DynamicDialogConfig, DynamicDialogRef } from 'primeng/dynamicdialog';
import { ImageModule } from 'primeng/image';
//import { AdminApiShortLinkApiClient, AdminApiTestApiClient, CreateShortLinkDto } from 'src/app/api/admin-api.service.generatesrc';


@Component({
    templateUrl: './congtrutientk.component.html',
    standalone: true,
    imports: [ContainerComponent,DropdownItemDirective,DropdownMenuDirective,DropdownToggleDirective, RowComponent,DropdownComponent, ColComponent, TextColorDirective, CardComponent,CardHeaderComponent, CardBodyComponent, FormDirective, InputGroupComponent, InputGroupTextDirective, IconDirective, FormControlDirective, ButtonDirective,CommonModule,ReactiveFormsModule,FormsModule,TableDirective,FormFloatingDirective,FormControlDirective, FormLabelDirective,FormSelectDirective,ImageModule]
})
export class CongTruTienTKComponent implements OnInit, OnDestroy{
    //System variables
    private ngUnsubscribe = new Subject<void>();
    constructor(private fb: FormBuilder,
      private router: Router,
      private alertService: AlertService,
      private userApiClient: AdminApiUserApiClient,
      public config: DynamicDialogConfig,
      public ref: DynamicDialogRef
    ) {
    }
    ngOnDestroy(): void {
      if (this.ref) {
        this.ref.close();
      }
      this.ngUnsubscribe.next();
      this.ngUnsubscribe.complete();
    }
  
    ngOnInit() { 
    }
    
    congtrutien(){
      
    }
}