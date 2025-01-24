import { DOCUMENT, NgStyle ,CommonModule} from '@angular/common';
import { Component, DestroyRef, effect, inject, OnDestroy, OnInit, Renderer2, signal, WritableSignal } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule,FormsModule } from '@angular/forms';
import { ChartOptions } from 'chart.js';
import { CalendarModule } from 'primeng/calendar';
import {
  AvatarComponent,
  ButtonDirective,
  ButtonGroupComponent,
  CardBodyComponent,
  CardComponent,
  CardFooterComponent,
  CardHeaderComponent,
  ColComponent,
  FormCheckLabelDirective,
  GutterDirective,
  ProgressBarDirective,
  ProgressComponent,
  RowComponent,
  TableDirective,
  TextColorDirective,
  FormControlDirective
} from '@coreui/angular';
import { ChartjsComponent } from '@coreui/angular-chartjs';
import { IconDirective } from '@coreui/icons-angular';
import { DashboardChartsData, IChartProps } from './dashboard-charts-data';
import { Subject, takeUntil } from 'rxjs';
import { UtilityService } from '../../shared/services/utility.service';
import { AlertService } from '../../shared/services/alert.service';

import { AdminApiCampainApiClient, AdminApiKeySearchApiClient, AdminApiShortLinkApiClient, ShortLinkInListDto, ShortLinkInListDtoPagedResult, ThongKeView, ThongKeViewClick } from '../../api/admin-api.service.generated';

@Component({
  templateUrl: 'dashboard.component.html',
  styleUrls: ['dashboard.component.scss'],
  standalone: true,
  imports: [ TextColorDirective, CardComponent, CardBodyComponent,FormsModule, RowComponent,FormControlDirective, ColComponent, ButtonDirective, IconDirective, ReactiveFormsModule, ButtonGroupComponent, FormCheckLabelDirective, ChartjsComponent, NgStyle, CardFooterComponent, GutterDirective, ProgressBarDirective, ProgressComponent, CardHeaderComponent, TableDirective, AvatarComponent,CalendarModule,CommonModule]
})
export class DashboardComponent implements OnInit, OnDestroy {

  readonly #destroyRef: DestroyRef = inject(DestroyRef);
  readonly #document: Document = inject(DOCUMENT);
  readonly #renderer: Renderer2 = inject(Renderer2);
  readonly #chartsData: DashboardChartsData = inject(DashboardChartsData);
  //System variables
  private ngUnsubscribe = new Subject<void>();
  //public items: ShortLinkInListDto[];
  public thongkeviewconlai: string = "0";
  public balance: string = "0";
  public hoahong: string = "0";
  public tongview: string = "0";
  public tongclick: string = "0";
  public tongthunhap: string = "0";
  public stringDate: string;
  public rangeDates: Date[] = [new Date(),new Date()];
  public from: Date;
  public to: Date;
  public isDateRange: Boolean = false;
  constructor(private alertService: AlertService,
    private shortlinkApiClient: AdminApiShortLinkApiClient,
    private thognkeApiClient: AdminApiCampainApiClient,
    private utilService: UtilityService,
  ) {}

  public mainChart: IChartProps = { type: 'line' };
  public mainChartRef: WritableSignal<any> = signal(undefined);
  #mainChartRefEffect = effect(() => {
    if (this.mainChartRef()) {
      this.setChartStyles();
    }
  });
  public chart: Array<IChartProps> = [];
  public trafficRadioGroup = new FormGroup({
    trafficRadio: new FormControl('Day')
  });

  ngOnInit(): void {
    //this.initCharts();
    //this.updateChartOnColorModeChange();
    
    this.loadBalance();
    this.loadhoahong();
    //this.loadTopLink();
    this.loadTongViewByRangeDate('Day');
    this.loadThongkeView();
  }
  ngOnDestroy(): void {
    this.ngUnsubscribe.next();
    this.ngUnsubscribe.complete();
  }

  initCharts(): void {
    this.mainChart = this.#chartsData.mainChart;
  }
  // loadTopLink(){
  //   this.shortlinkApiClient.getTopLink()
  //   .pipe(takeUntil(this.ngUnsubscribe))
  //   .subscribe({
  //     next: (response: ShortLinkInListDto[]) => {
  //       this.items = response;
  //     },
  //     error: (error: any) => {
  //       console.log(error);
  //       this.alertService.showError('Có lỗi xảy ra');
  //     },
  //   });
  // }
  loadBalance(){
    this.shortlinkApiClient.getBalance()
    .pipe(takeUntil(this.ngUnsubscribe))
    .subscribe({
      next: (response: number) => {
        this.balance = new Intl.NumberFormat('vi').format(response);
      },
      error: (error: any) => {
        console.log(error);
        this.alertService.showError('Có lỗi xảy ra');
      },
    });
  }
  loadThongkeView(){
    this.thognkeApiClient.getThongKeView()
    .pipe(takeUntil(this.ngUnsubscribe))
    .subscribe({
      next: (response: ThongKeView) => {
        this.thongkeviewconlai = new Intl.NumberFormat('vi').format(response.viewConLaiTrongNgay!);
      },
      error: (error: any) => {
        console.log(error);
        this.alertService.showError('Có lỗi xảy ra');
      },
    });
  }
  loadTongViewByRangeDate(value: string){
    this.getStringDate(value);
    this.shortlinkApiClient.getThongKeClickByDate(this.from,this.to)
    .pipe(takeUntil(this.ngUnsubscribe))
    .subscribe({
      next: (response: ThongKeViewClick) => {
        this.tongclick = new Intl.NumberFormat('vi').format(response.click!);
        this.tongthunhap = new Intl.NumberFormat('vi').format(response.click! *1000);
        this.tongview = new Intl.NumberFormat('vi').format(response.view!);
      },
      error: (error: any) => {
        console.log(error);
        this.alertService.showError('Có lỗi xảy ra');
      },
    });
  }
  setTrafficPeriod(value: string): void {
    if (value != "CustomDay") {
      this.isDateRange = false;
      this.loadTongViewByRangeDate(value);
    }
    else{
      this.isDateRange = true;
    }
    
  }
  getStringDate(value: string){
    var currentDateFrom = new Date();
    var currentDateTo = new Date();
    if (value == 'Day') {
      currentDateFrom.setHours(0,0,0,0);
      this.from = currentDateFrom;
      currentDateTo.setHours(0,0,0,0);
      currentDateTo.setDate(currentDateTo.getDate() + 1)
      this.to = currentDateTo;
    } else if(value == 'Yesterday') {
      currentDateFrom.setHours(0,0,0,0);
      this.to = currentDateFrom;
      currentDateTo.setHours(0,0,0,0);
      currentDateTo.setDate(currentDateTo.getDate() - 1)
      this.from = currentDateTo;
    }
    else{
    }
    this.stringDate = 'Từ ngày ' + this.utilService.getDateFormat(this.from) + ' đến ' + this.utilService.getDateFormat(this.to)
  }

  handleChartRef($chartRef: any) {
    if ($chartRef) {
      this.mainChartRef.set($chartRef);
    }
  }

  updateChartOnColorModeChange() {
    const unListen = this.#renderer.listen(this.#document.documentElement, 'ColorSchemeChange', () => {
      this.setChartStyles();
    });

    this.#destroyRef.onDestroy(() => {
      unListen();
    });
  }

  setChartStyles() {
    if (this.mainChartRef()) {
      setTimeout(() => {
        const options: ChartOptions = { ...this.mainChart.options };
        const scales = this.#chartsData.getScales();
        this.mainChartRef().options.scales = { ...options.scales, ...scales };
        this.mainChartRef().update();
      });
    }
  }
  changeDateRange(event){
    if (event[1] != null) {
      this.from = event[0];
      this.to = event[1];
      this.loadTongViewByRangeDate("");
    }
    
  }
  loadhoahong(){
    this.shortlinkApiClient.getHoaHongByDate(this.from,this.to)
    .pipe(takeUntil(this.ngUnsubscribe))
    .subscribe({
      next: (response: number) => {
        this.hoahong = new Intl.NumberFormat('vi').format(response);
      },
      error: (error: any) => {
        console.log(error);
        this.alertService.showError('Có lỗi xảy ra');
      },
    });
    
  }
}
