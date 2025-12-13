import { ComponentFixture, TestBed } from '@angular/core/testing';

import { Administrator } from './administrator';

describe('Administrator', () => {
  let component: Administrator;
  let fixture: ComponentFixture<Administrator>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [Administrator]
    })
    .compileComponents();

    fixture = TestBed.createComponent(Administrator);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
