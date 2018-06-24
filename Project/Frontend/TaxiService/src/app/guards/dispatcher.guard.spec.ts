import { TestBed, async, inject } from '@angular/core/testing';

import { DispatcherGuard } from './dispatcher.guard';

describe('DispatcherGuard', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [DispatcherGuard]
    });
  });

  it('should ...', inject([DispatcherGuard], (guard: DispatcherGuard) => {
    expect(guard).toBeTruthy();
  }));
});
