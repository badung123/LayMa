import { Component,OnDestroy, OnInit} from '@angular/core';
import {
  FormBuilder,
  FormControl,
  FormGroup,
  ReactiveFormsModule,
  Validators
} from '@angular/forms';
import { IconDirective } from '@coreui/icons-angular';
import { ContainerComponent, RowComponent, ColComponent, TextColorDirective, CardComponent,CardHeaderComponent, CardBodyComponent, FormDirective, InputGroupComponent, InputGroupTextDirective, FormControlDirective, ButtonDirective } from '@coreui/angular';
import { AdminApiShortLinkApiClient, UpdateNguon } from '../../../api/admin-api.service.generated';
import { AlertService } from '../../../shared/services/alert.service';
import { UrlConstants } from '../../../shared/constants/url.constants';
import { Router } from '@angular/router';
import { DynamicDialogConfig, DynamicDialogRef } from 'primeng/dynamicdialog';
import { Subject } from 'rxjs';

@Component({
    templateUrl: './shortlinknote.component.html',
    standalone: true,
    imports: [ContainerComponent, RowComponent, ColComponent, TextColorDirective, CardComponent,CardHeaderComponent, CardBodyComponent, FormDirective, InputGroupComponent, InputGroupTextDirective, IconDirective, FormControlDirective, ButtonDirective,ReactiveFormsModule]
})
export class ShortlinkNoteComponent implements OnInit, OnDestroy{
    shortlinkNoteForm: FormGroup;
    private ngUnsubscribe = new Subject<void>();
    public link : string ;
    constructor(private fb: FormBuilder,
      private shortLinkApi: AdminApiShortLinkApiClient,
      private alertService: AlertService,
      public config: DynamicDialogConfig,
      public ref: DynamicDialogRef,
      private router: Router) {
      this.shortlinkNoteForm = this.fb.group({
        origin: new FormControl('', Validators.required),
        
      });
      
    }
    ngOnDestroy(): void {
      if (this.ref) {
        this.ref.close();
      }
      this.ngUnsubscribe.next();
      this.ngUnsubscribe.complete();
    }
    
      ngOnInit() {
        this.link = this.config.data.link;   
      }
    updateOrigin() {

        var request: UpdateNguon = new UpdateNguon({
            shortlinkId : this.config.data.id,
            origin: this.shortlinkNoteForm.controls['origin'].value
        });
      this.shortLinkApi.updateNguon(request).subscribe({
        next: () => {
          this.alertService.showSuccess('Đã thêm nguồn thành công');
          this.ref.close();
        },
        error: (error: any) => {
          console.log(error);
          this.alertService.showError('Có lỗi xảy ra');
        },
      });
    }
    

}