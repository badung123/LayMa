import { Component } from '@angular/core';
import {
  FormBuilder,
  FormControl,
  FormGroup,
  ReactiveFormsModule,
  Validators
} from '@angular/forms';
import { IconDirective } from '@coreui/icons-angular';
import { ContainerComponent, RowComponent, ColComponent, TextColorDirective, CardComponent,CardHeaderComponent, CardBodyComponent, FormDirective, InputGroupComponent, InputGroupTextDirective, FormControlDirective, ButtonDirective } from '@coreui/angular';
import { AdminApiShortLinkApiClient, CreateShortLinkDto } from '../../../api/admin-api.service.generated';
import { AlertService } from '../../../shared/services/alert.service';
import { UrlConstants } from '../../../shared/constants/url.constants';
import { Router } from '@angular/router';

@Component({
    selector: 'app-shortlink',
    templateUrl: './shortlink.component.html',
    styleUrls: ['./shortlink.component.scss'],
    standalone: true,
    imports: [ContainerComponent, RowComponent, ColComponent, TextColorDirective, CardComponent,CardHeaderComponent, CardBodyComponent, FormDirective, InputGroupComponent, InputGroupTextDirective, IconDirective, FormControlDirective, ButtonDirective,ReactiveFormsModule]
})
export class ShortLinkComponent {
    shortlinkForm: FormGroup;
    constructor(private fb: FormBuilder,
      private shortLinkApi: AdminApiShortLinkApiClient,
      private alertService: AlertService,
      private router: Router) {
      this.shortlinkForm = this.fb.group({
        shortlink: new FormControl('', Validators.required),
      });
    }
    createlink() {

        var request: CreateShortLinkDto = new CreateShortLinkDto({
            url : this.shortlinkForm.controls['shortlink'].value
        });
      this.shortLinkApi.createShortLink(request).subscribe({
        next: () => {
          this.router.navigate([UrlConstants.LiST_SHORT_LINK]);
        },
        error: (error: any) => {
          console.log(error);
          this.alertService.showError('Có lỗi xảy ra');
        },
      });
    }

}