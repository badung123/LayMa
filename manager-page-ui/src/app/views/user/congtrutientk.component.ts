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
import { AdminApiUserApiClient, AdminBalanceAdjustmentRequest, UserDtoInList } from '../../api/admin-api.service.generated';
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


@Component({
    templateUrl: './congtrutientk.component.html',
    styleUrls: ['./congtrutientk.component.scss'],
    standalone: true,
    imports: [ContainerComponent,DropdownItemDirective,DropdownMenuDirective,DropdownToggleDirective, RowComponent,DropdownComponent, ColComponent, TextColorDirective, CardComponent,CardHeaderComponent, CardBodyComponent, FormDirective, InputGroupComponent, InputGroupTextDirective, IconDirective, FormControlDirective, ButtonDirective,CommonModule,ReactiveFormsModule,FormsModule,TableDirective,FormFloatingDirective,FormControlDirective, FormLabelDirective,FormSelectDirective,ImageModule]
})
export class CongTruTienTKComponent implements OnInit, OnDestroy{
    //System variables
    balanceAdjustmentForm: FormGroup;
    private ngUnsubscribe = new Subject<void>();
    public userInfo: UserDtoInList;
    public isSubmitting: boolean = false;
    
    constructor(private fb: FormBuilder,
      private router: Router,
      private alertService: AlertService,
      private userApiClient: AdminApiUserApiClient,
      public config: DynamicDialogConfig,
      public ref: DynamicDialogRef
    ) {
      this.userInfo = this.config.data?.user;
    }
    
    ngOnDestroy(): void {
      if (this.ref) {
        this.ref.close();
      }
      this.ngUnsubscribe.next();
      this.ngUnsubscribe.complete();
    }
  
    ngOnInit() { 
      this.initializeForm();
    }
    
    private initializeForm(): void {
      this.balanceAdjustmentForm = this.fb.group({
        amount: new FormControl('', [
          Validators.required,
          Validators.min(0.01),
          Validators.pattern(/^\d+(\.\d{1,2})?$/)
        ]),
        description: new FormControl('', [
          Validators.required,
          Validators.minLength(5),
          Validators.maxLength(500)
        ]),
        operationType: new FormControl('add', Validators.required)
      });
    }
    
    get formControls() {
      return this.balanceAdjustmentForm.controls;
    }
    
    onAmountChange(): void {
      const amount = this.balanceAdjustmentForm.get('amount')?.value;
      if (amount && amount < 0) {
        this.balanceAdjustmentForm.get('amount')?.setValue(Math.abs(amount));
      }
    }
    
    adjustBalance(): void {
      if (this.balanceAdjustmentForm.invalid) {
        this.markFormGroupTouched();
        return;
      }
      
      this.isSubmitting = true;
      
      const formValue = this.balanceAdjustmentForm.value;
      const amount = parseFloat(formValue.amount);
      const finalAmount = formValue.operationType === 'subtract' ? -amount : amount;
      
      const request: AdminBalanceAdjustmentRequest = new AdminBalanceAdjustmentRequest({
        userId: this.userInfo.id!,
        amount: finalAmount,
        description: formValue.description
      });
      
      this.userApiClient.adjustUserBalanceByAdmin(request)
        .pipe(takeUntil(this.ngUnsubscribe))
        .subscribe({
          next: (response: any) => {
            this.alertService.showSuccess(
              `Đã ${formValue.operationType === 'add' ? 'cộng' : 'trừ'} ${amount.toLocaleString('vi-VN')} VNĐ thành công!`
            );
            this.ref.close(true); // Pass true to indicate success
          },
          error: (error: any) => {
            console.error('Error adjusting balance:', error);
            this.alertService.showError('Có lỗi xảy ra khi thực hiện giao dịch');
            this.isSubmitting = false;
          },
        });
    }
    
    private markFormGroupTouched(): void {
      Object.keys(this.balanceAdjustmentForm.controls).forEach(key => {
        const control = this.balanceAdjustmentForm.get(key);
        control?.markAsTouched();
      });
    }
    
    cancel(): void {
      this.ref.close(false);
    }
    
    getCurrentBalance(): string {
      return this.userInfo?.balance?.toLocaleString('vi-VN') || '0';
    }
    
    getOperationText(): string {
      const operationType = this.balanceAdjustmentForm.get('operationType')?.value;
      return operationType === 'add' ? 'Cộng tiền' : 'Trừ tiền';
    }
}