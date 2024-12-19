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
import { UtilityService } from 'src/app/shared/services/utility.service';
import { AlertService } from 'src/app/shared/services/alert.service';
import { AdminApiCampainApiClient, AdminApiKeySearchApiClient, AdminApiShortLinkApiClient, ShortLinkInListDto, ShortLinkInListDtoPagedResult, ThongKeView, ThongKeViewClick } from 'src/app/api/admin-api.service.generated';

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
  public items: ShortLinkInListDto[];
  public thongkeviewconlai: number = 0;
  public balance: number = 0;
  public hoahong: number = 0;
  public tongview: number = 0;
  public tongclick: number = 0;
  public tongthunhap: number = 0;
  public stringDate: string;
  public rangeDates: any;
  public from: Date;
  public to: Date;
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
    this.loadTopLink();
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
  loadTopLink(){
    this.shortlinkApiClient.getTopLink()
    .pipe(takeUntil(this.ngUnsubscribe))
    .subscribe({
      next: (response: ShortLinkInListDto[]) => {
        this.items = response;
      },
      error: (error: any) => {
        console.log(error);
        this.alertService.showError('Có lỗi xảy ra');
      },
    });
  }
  loadBalance(){
    this.shortlinkApiClient.getBalance()
    .pipe(takeUntil(this.ngUnsubscribe))
    .subscribe({
      next: (response: number) => {
        this.balance = response;
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
        this.thongkeviewconlai = response.viewConLaiTrongNgay;
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
        this.tongclick = response.click;
        this.tongthunhap = response.click *1000;
        this.tongview = response.view;
      },
      error: (error: any) => {
        console.log(error);
        this.alertService.showError('Có lỗi xảy ra');
      },
    });
  }
  setTrafficPeriod(value: string): void {
    
    if (value != "CustomDay") {
      this.loadTongViewByRangeDate(value);
    }
    
  }
  getStringDate(value: string){
    var currentDateFrom = new Date();
    var currentDateTo = new Date();
    if (value == 'Day') {
      currentDateFrom.setUTCHours(0,0,0,0);
      this.from = currentDateFrom;
      currentDateTo.setUTCHours(0,0,0,0);
      currentDateTo.setDate(currentDateTo.getDate() + 1)
      this.to = currentDateTo;
    } else if(value == 'Yesterday') {
      currentDateFrom.setUTCHours(0,0,0,0);
      this.to = currentDateFrom;
      currentDateTo.setUTCHours(0,0,0,0);
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
    console.log(event);
    console.log(this.rangeDates);
  }
}
