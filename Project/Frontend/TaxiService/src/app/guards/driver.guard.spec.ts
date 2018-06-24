import { TestBed, async, inject } from '@angular/core/testing';

import { DriverGuard } from './driver.guard';

describe('DriverGuard', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [DriverGuard]
    });
  });

  it('should ...', inject([DriverGuard], (guard: DriverGuard) => {
    expect(guard).toBeTruthy();
  }));
});
