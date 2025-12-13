import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RoleChanger } from './role-changer';

describe('RoleChanger', () => {
  let component: RoleChanger;
  let fixture: ComponentFixture<RoleChanger>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [RoleChanger]
    })
    .compileComponents();

    fixture = TestBed.createComponent(RoleChanger);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
