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
import { AdminApiUserApiClient,ProcessStatus, UserDtoInList, UserDtoInListPagedResult } from '../../api/admin-api.service.generated';
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
import { DialogService, DynamicDialogComponent } from 'primeng/dynamicdialog';
import { XacMinhUserComponent } from './xacminhuser.component';
import { PaginatorModule } from 'primeng/paginator';
//import { AdminApiShortLinkApiClient, AdminApiTestApiClient, CreateShortLinkDto } from 'src/app/api/admin-api.service.generatesrc';


@Component({
    selector: 'app-user',
    templateUrl: './user.component.html',
    styleUrls: ['./user.component.scss'],
    standalone: true,
    imports: [ContainerComponent,DropdownItemDirective,PaginatorModule,DropdownMenuDirective,DropdownToggleDirective, RowComponent,DropdownComponent, ColComponent, TextColorDirective, CardComponent,CardHeaderComponent, CardBodyComponent, FormDirective, InputGroupComponent, InputGroupTextDirective, IconDirective, FormControlDirective, ButtonDirective,CommonModule,ReactiveFormsModule,FormsModule,TableDirective,FormFloatingDirective,FormControlDirective, FormLabelDirective,FormSelectDirective]
})
export class UserComponent implements OnInit, OnDestroy{
    //System variables
    private ngUnsubscribe = new Subject<void>();
    //Paging variables
    public pageIndex: number = 1;
    public pageSize: number = 10;
    public totalCount: number;

    //Business variables
    public items: UserDtoInList[];
    public keyword: string = '';
    public accountName: string;
    public processStatus = ProcessStatus;
    constructor(private fb: FormBuilder,
      private router: Router,
      private alertService: AlertService,
      private userApiClient: AdminApiUserApiClient,
      public dialogService: DialogService
    ) {
    }
    ngOnDestroy(): void {
      this.ngUnsubscribe.next();
      this.ngUnsubscribe.complete();
    }
  
    ngOnInit() { 
      this.loadData();  
    }
    loadData(){
      this.userApiClient.getListUser(this.pageIndex,this.pageSize,this.keyword)
      .pipe(takeUntil(this.ngUnsubscribe))
      .subscribe({
        next: (response: UserDtoInListPagedResult) => {
          this.items = response.results!;
          this.totalCount = response.rowCount!;
        },
        error: (error: any) => {
          console.log(error);
          this.alertService.showError('Có lỗi xảy ra');
        },
      });
    }
    pageChanged(event: any): void {
      this.pageIndex = event.page + 1;
      this.pageSize = event.rows;
      this.loadData();
    }
    xacminh(isVerify: Boolean,id: string,origin:string,originImage:string){
      if (isVerify) return;
      if (originImage == null || originImage == ''){
        this.alertService.showError("User chưa lên lệnh xác minh")
        return;
      }
      const ref = this.dialogService.open(XacMinhUserComponent, {
        data: {
          origin: origin,
          id: id,
          originImage:originImage
        },
        header: 'Xác minh tài khoản',
        width: '70%'
      });
      const dialogRef = this.dialogService.dialogComponentRefMap.get(ref);
      const dynamicComponent = dialogRef?.instance as DynamicDialogComponent;
      const ariaLabelledBy = dynamicComponent.getAriaLabelledBy();
      dynamicComponent.getAriaLabelledBy = () => ariaLabelledBy;
      ref.onClose.subscribe((data: any) => {
        this.loadData();  
      });

    }
}