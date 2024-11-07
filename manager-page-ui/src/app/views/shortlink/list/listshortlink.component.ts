import { Component, OnDestroy, OnInit } from '@angular/core';
import { IconDirective } from '@coreui/icons-angular';
import { ContainerComponent, RowComponent, ColComponent, TextColorDirective, CardComponent, CardBodyComponent, FormDirective, InputGroupComponent, InputGroupTextDirective, FormControlDirective, ButtonDirective, CardHeaderComponent, TableDirective } from '@coreui/angular';
import { Subject, takeUntil } from 'rxjs';
import { AlertService } from 'src/app/shared/services/alert.service';
import { AdminApiShortLinkApiClient, ShortLinkInListDto, ShortLinkInListDtoPagedResult } from 'src/app/api/admin-api.service.generated';
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
//import { AdminApiShortLinkApiClient, AdminApiTestApiClient, CreateShortLinkDto } from 'src/app/api/admin-api.service.generatesrc';

@Component({
    selector: 'app-listshortlink',
    templateUrl: './listshortlink.component.html',
    styleUrls: ['./listshortlink.component.scss'],
    standalone: true,
    imports: [ContainerComponent, RowComponent, ColComponent, TextColorDirective, CardComponent,CardHeaderComponent, CardBodyComponent, FormDirective, InputGroupComponent, InputGroupTextDirective, IconDirective, FormControlDirective, ButtonDirective,CommonModule,ReactiveFormsModule,FormsModule,TableDirective]
})
export class ListShortLinkComponent implements OnInit, OnDestroy{
    //System variables
    private ngUnsubscribe = new Subject<void>();
    //Paging variables
    public pageIndex: number = 1;
    public pageSize: number = 5;
    public totalCount: number;

    //Business variables
    public items: ShortLinkInListDto[];
    public keyword: string = '';
    constructor(private alertService: AlertService,
      private shortlinkApiClient: AdminApiShortLinkApiClient
    ) {}
    ngOnDestroy(): void {
      this.ngUnsubscribe.next();
      this.ngUnsubscribe.complete();
    }
  
    ngOnInit() {
      this.loadData();    
    }
    loadData() {
      this.shortlinkApiClient.getPostsPaging2(this.pageIndex,this.pageSize,this.keyword)
      .pipe(takeUntil(this.ngUnsubscribe))
      .subscribe({
        next: (response: ShortLinkInListDtoPagedResult) => {
          this.items = response.results;
          this.totalCount = response.rowCount;
          console.log(this.items);
        },
        error: (error: any) => {
          console.log(error);
          this.alertService.showError('Có lỗi xảy ra');
        },
      });
    }
    

}