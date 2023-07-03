import { Injectable } from '@angular/core';
import { NgxSpinnerService } from 'ngx-spinner';

@Injectable({
  providedIn: 'root'
})
export class LoadingService {
  lodingRequestCount = 0;

  constructor(private spinnerService: NgxSpinnerService) { }

  loding() {
    this.lodingRequestCount++;
    this.spinnerService.show(undefined, {
      type: 'line-scale-party',
      bdColor: 'rgba(255,255,255,0)',
      color: '#333333'
    })
  }

  idle() {
    this.lodingRequestCount--;
    if(this.lodingRequestCount <= 0) {
      this.lodingRequestCount = 0;
      this.spinnerService.hide();
    }
  }
}
