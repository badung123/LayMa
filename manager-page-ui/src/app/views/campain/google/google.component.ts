import { Component, OnDestroy, OnInit } from '@angular/core';
import {
  FormBuilder,
  FormControl,
  FormGroup,
  ReactiveFormsModule,
  Validators
} from '@angular/forms';
import { IconDirective } from '@coreui/icons-angular';
import { ContainerComponent, RowComponent, ColComponent, TextColorDirective, CardComponent,CardHeaderComponent, CardBodyComponent, FormDirective, InputGroupComponent, InputGroupTextDirective, FormControlDirective, ButtonDirective, TableDirective } from '@coreui/angular';
import { Router } from '@angular/router';
import { DialogService, DynamicDialogComponent } from 'primeng/dynamicdialog';
import { AlertService } from '../../../shared/services/alert.service';
import { CampainComponent } from '../campain.component';
import { Subject, takeUntil } from 'rxjs';
import { AdminApiCampainApiClient, CampainInListDto, CampainInListDtoPagedResult } from '../../../api/admin-api.service.generated';
import { CommonModule } from '@angular/common';
import { CampainDetailComponent } from '../campainDetail.component';
import { PaginatorModule } from 'primeng/paginator';

@Component({
    selector: 'app-google',
    templateUrl: './google.component.html',
    styleUrls: ['./google.component.scss'],
    standalone: true,
    imports: [ContainerComponent,PaginatorModule, RowComponent, ColComponent, TextColorDirective, CardComponent,CardHeaderComponent,TableDirective, CardBodyComponent, FormDirective, InputGroupComponent, InputGroupTextDirective, IconDirective, FormControlDirective, ButtonDirective,ReactiveFormsModule,CommonModule]
})
export class GoogleComponent implements OnInit, OnDestroy{
  private ngUnsubscribe = new Subject<void>();
  //Paging variables
      public pageIndex: number = 1;
      public pageSize: number = 10;
      public totalCount: number;
  
      //Business variables
      public items: CampainInListDto[];
      public keyword: string = '';
    constructor(public dialogService: DialogService,
      private alertService: AlertService,
      private campainApiClient: AdminApiCampainApiClient
    ) {
    }
    ngOnDestroy(): void {
      this.ngUnsubscribe.next();
      this.ngUnsubscribe.complete();
    }
  
    ngOnInit() {
      this.loadData();
    }
    showAddModal(){
      const ref = this.dialogService.open(CampainComponent, {
        data: {
          flatform:'google'
        },
        header: 'Thêm mới chiến dịch google',
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
    loadData(){
      this.campainApiClient
              .getPostsPaging2(this.pageIndex,this.pageSize,'google',this.keyword)
              .pipe(takeUntil(this.ngUnsubscribe))
              .subscribe({
                next: (response: CampainInListDtoPagedResult) => {
                  this.items = response.results!;
                  this.totalCount = response.rowCount!;
                  console.log(this.items);
                },
                error: () => {
                  this.alertService.showError('Có lỗi xảy ra');
                },
              });
    }
    pageChanged(event: any): void {
      this.pageIndex = event.page + 1;
      this.pageSize = event.rows;
      this.loadData();
    }
    showEditModal(id: string){
      const ref = this.dialogService.open(CampainComponent, {
        data: {
          id:id,
          flatform:'google'
        },
        header: 'Sửa chiến dịch google',
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
    showModalDetail(id: string,keyToken: string){
      const ref = this.dialogService.open(CampainDetailComponent, {
        data: {
          id:id,
          keyToken:keyToken
        },
        header: 'Hướng dẫn tích hợp chiến dịch',
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