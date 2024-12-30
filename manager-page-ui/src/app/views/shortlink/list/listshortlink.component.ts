import { Component, OnDestroy, OnInit } from '@angular/core';
import { IconDirective } from '@coreui/icons-angular';
import { ContainerComponent, RowComponent, ColComponent, TextColorDirective, CardComponent, CardBodyComponent, FormDirective, InputGroupComponent, InputGroupTextDirective, FormControlDirective, ButtonDirective, CardHeaderComponent, TableDirective } from '@coreui/angular';
import { Subject, takeUntil } from 'rxjs';
import { AlertService } from '../../../shared/services/alert.service';
import { AdminApiShortLinkApiClient, ShortLinkInListDto, ShortLinkInListDtoPagedResult } from '../../../api/admin-api.service.generated';
import { CommonModule, NgStyle } from '@angular/common';
import { DialogService, DynamicDialogComponent } from 'primeng/dynamicdialog';
import {
  FormBuilder,
  FormControl,
  FormGroup,
  FormsModule,
  NgModel,
  ReactiveFormsModule,
  Validators
} from '@angular/forms';
import { ShortlinkNoteComponent } from './shortlinknote.component';
import { PaginatorModule } from 'primeng/paginator';
//import { AdminApiShortLinkApiClient, AdminApiTestApiClient, CreateShortLinkDto } from 'src/app/api/admin-api.service.generatesrc';

@Component({
    selector: 'app-listshortlink',
    templateUrl: './listshortlink.component.html',
    styleUrls: ['./listshortlink.component.scss'],
    standalone: true,
    imports: [ContainerComponent,PaginatorModule, RowComponent, ColComponent, TextColorDirective, CardComponent,CardHeaderComponent, CardBodyComponent, FormDirective, InputGroupComponent, InputGroupTextDirective, IconDirective, FormControlDirective, ButtonDirective,CommonModule,ReactiveFormsModule,FormsModule,TableDirective,ShortlinkNoteComponent]
})
export class ListShortLinkComponent implements OnInit, OnDestroy{
    //System variables
    private ngUnsubscribe = new Subject<void>();
    //Paging variables
    public pageIndex: number = 1;
    public pageSize: number = 10;
    public totalCount: number;

    //Business variables
    public items: ShortLinkInListDto[];
    public keyword: string = '';
    constructor(private alertService: AlertService,
      private shortlinkApiClient: AdminApiShortLinkApiClient,
      public dialogService: DialogService,
    ) {}
    ngOnDestroy(): void {
      this.ngUnsubscribe.next();
      this.ngUnsubscribe.complete();
    }
  
    ngOnInit() {
      this.loadData();    
    }
    loadData() {
      this.shortlinkApiClient.getPostsPaging3(this.pageIndex,this.pageSize,this.keyword)
      .pipe(takeUntil(this.ngUnsubscribe))
      .subscribe({
        next: (response: ShortLinkInListDtoPagedResult) => {
          this.items = response.results!;
          this.totalCount = response.rowCount!;
          console.log(this.items);
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
    showModalUpdateNguon(shortlinkId: string,link: string ){
      const ref = this.dialogService.open(ShortlinkNoteComponent, {
        data: {
          id: shortlinkId,
          link: link
        },
        header: 'Cập nhật nguồn link rút gọn',
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