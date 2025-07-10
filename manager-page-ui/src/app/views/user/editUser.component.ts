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
import { AdminApiUserApiClient,ProcessStatus, UpdateClickRateRequest, UserDtoInList, UserDtoInListPagedResult, VerifyOrLockUserRequest } from '../../api/admin-api.service.generated';
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
    templateUrl: './editUser.component.html',
    standalone: true,
    imports: [ContainerComponent,DropdownItemDirective,DropdownMenuDirective,DropdownToggleDirective, RowComponent,DropdownComponent, ColComponent, TextColorDirective, CardComponent,CardHeaderComponent, CardBodyComponent, FormDirective, InputGroupComponent, InputGroupTextDirective, IconDirective, FormControlDirective, ButtonDirective,CommonModule,ReactiveFormsModule,FormsModule,TableDirective,FormFloatingDirective,FormControlDirective, FormLabelDirective,FormSelectDirective,ImageModule]
})
export class EditUserComponent implements OnInit, OnDestroy{
    //System variables
    editAccountForm: FormGroup;
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
      this.editAccountForm = this.fb.group({
              click: new FormControl(this.config.data?.maxClick, Validators.required),
              rate:new FormControl(this.config.data?.rate, Validators.required)             
            });
    }
    
    editData(){
      
      
      var request: UpdateClickRateRequest = new UpdateClickRateRequest({
        userId: this.config.data?.id,
        maxClickInDay: this.editAccountForm.controls['click'].value,
        rate: this.editAccountForm.controls['rate'].value,
      }); 
      this.userApiClient.updateUserClickRate(request)
      .pipe(takeUntil(this.ngUnsubscribe))
      .subscribe({
        next: (response: any) => {
          this.alertService.showSuccess('Cập nhật thành công');
          this.ref.close();
          //reload
        },
        error: (error: any) => {
          console.log(error);
          this.alertService.showError('Có lỗi xảy ra');
        },
      });    
    }
}