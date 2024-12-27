import { Component, OnDestroy, OnInit } from '@angular/core';
import {
  FormBuilder,
  FormControl,
  FormGroup,
  ReactiveFormsModule,
  Validators
} from '@angular/forms';
import { IconDirective } from '@coreui/icons-angular';
import { ContainerComponent, RowComponent, ColComponent, TextColorDirective, CardComponent,CardHeaderComponent, CardBodyComponent, FormDirective, InputGroupComponent, InputGroupTextDirective, FormControlDirective, ButtonDirective } from '@coreui/angular';
import { Router } from '@angular/router';
import { DialogService, DynamicDialogComponent, DynamicDialogConfig, DynamicDialogRef } from 'primeng/dynamicdialog';
import { AlertService } from '../../shared/services/alert.service';
import { Subject, takeUntil } from 'rxjs';
import { UploadService } from '../../shared/services/upload.service';
import { environment } from '../../../environments/environment';
import { ImageModule } from 'primeng/image';
import { CommonModule } from '@angular/common';
import { AdminApiCampainApiClient, CampainInListDto, CreateOrUpdateCampainRequest } from '../../api/admin-api.service.generated';
import { UtilityService } from '../../shared/services/utility.service';

@Component({
    templateUrl: './campainDetail.component.html',
    standalone: true,
    imports: [ContainerComponent, RowComponent, ColComponent, TextColorDirective, CardComponent,CardHeaderComponent, CardBodyComponent, FormDirective, InputGroupComponent, InputGroupTextDirective, IconDirective, FormControlDirective, ButtonDirective,ReactiveFormsModule,ImageModule,CommonModule]
})
export class CampainDetailComponent implements OnInit, OnDestroy{
    private ngUnsubscribe = new Subject<void>();
    public linkJs: string = "";
    public linkTagHtml: string = "";
    constructor(public dialogService: DialogService,
      private alertService: AlertService,
      private fb: FormBuilder,
      public config: DynamicDialogConfig,
      public ref: DynamicDialogRef,
      private uploadService: UploadService,
      private utilService: UtilityService,
      private campainApiClient: AdminApiCampainApiClient
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

}