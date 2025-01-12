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
    templateUrl: './campain.component.html',
    standalone: true,
    imports: [ContainerComponent, RowComponent, ColComponent, TextColorDirective, CardComponent,CardHeaderComponent, CardBodyComponent, FormDirective, InputGroupComponent, InputGroupTextDirective, IconDirective, FormControlDirective, ButtonDirective,ReactiveFormsModule,ImageModule,CommonModule]
})
export class CampainComponent implements OnInit, OnDestroy{
    campainForm: FormGroup;
    private ngUnsubscribe = new Subject<void>();
    public thumbnailImage;
    public selectedEntity = {} as CampainInListDto;
    public isGoogle = true;
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
      this.buildForm();
      if (this.utilService.isEmpty(this.config.data?.id) == false) {
        this.loadFormDetails(this.config.data?.id);
      }
      this.isGoogle = this.config.data?.flatform == 'google' ? true : false;
    }
    onFileChange(event) {
      if (event.target.files && event.target.files.length) {
        this.uploadService.uploadImage('posts', event.target.files)
          .subscribe({
            next: (response: any) => {
              this.campainForm.controls['thumbnail'].setValue(response.path);
              this.thumbnailImage = environment.API_URL + response.path;
            },
            error: (err: any) => {
              console.log(err);
            }
          });
      }
    }
    buildForm(){
      this.campainForm = this.fb.group({
        key: new FormControl(this.selectedEntity.keySearch || '', Validators.required),
        urlWeb: new FormControl(this.selectedEntity.url || '', Validators.required),
        price: new FormControl(this.selectedEntity.pricePerView || '', Validators.required),
        view: new FormControl(this.selectedEntity.viewPerDay || '', Validators.required),
        time: new FormControl(this.selectedEntity.timeOnSitePerView || '', Validators.required),
        domain: new FormControl(this.selectedEntity.domain || '', Validators.required),
        thumbnail: new FormControl(
          this.selectedEntity.imageUrl || null
        ),
        
      });
    }
    loadFormDetails(id: string) {
      this.campainApiClient
        .getCampainByCampainId(id)
        .pipe(takeUntil(this.ngUnsubscribe))
        .subscribe({
          next: (response: CampainInListDto) => {
            this.selectedEntity = response;
            this.buildForm();
            this.thumbnailImage = environment.API_URL + response.imageUrl;
          },
          error: () => {
            this.alertService.showError('Có lỗi xảy ra');
          },
        });
    }
    saveData() {
      //check các kiểu
      let key = "";
      if (this.isGoogle) {
        key = this.campainForm.controls['key'].value;
        if (key == null || key == '') {
          this.alertService.showError('Từ khóa không đc bỏ trống');
          return;
        }
      }     
      let urlWeb = this.campainForm.controls['urlWeb'].value;
      if (urlWeb == null || urlWeb == '') {
        urlWeb = '';
      }
      let domain = this.campainForm.controls['domain'].value;
      if (domain == null || domain == '') {
        this.alertService.showError('Domain không đc bỏ trống');
        return;
      }
      var request: CreateOrUpdateCampainRequest = new CreateOrUpdateCampainRequest({
        key : key,
        urlWeb: urlWeb,
        price: this.campainForm.controls['price'].value,
        time: this.campainForm.controls['time'].value,
        view: this.campainForm.controls['view'].value,
        thumbnail: this.campainForm.controls['thumbnail'].value,
        flatform: this.config.data?.flatform,
        domain:this.campainForm.controls['domain'].value
      });
      if (this.utilService.isEmpty(this.config.data?.id) == false) {
        request.campainId = this.config.data?.id;
      }
          
      this.campainApiClient
          .createOrUpdateCampain(request)
          .pipe(takeUntil(this.ngUnsubscribe))
          .subscribe({
            next: () => {
              this.ref.close();
            },
            error: () => {
            },
          }); 
    }

}