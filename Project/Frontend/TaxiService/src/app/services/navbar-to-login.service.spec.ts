import { TestBed, inject } from '@angular/core/testing';

import { NavbarToLoginService } from './navbar-to-login.service';

describe('NavbarToLoginService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [NavbarToLoginService]
    });
  });

  it('should be created', inject([NavbarToLoginService], (service: NavbarToLoginService) => {
    expect(service).toBeTruthy();
  }));
});
