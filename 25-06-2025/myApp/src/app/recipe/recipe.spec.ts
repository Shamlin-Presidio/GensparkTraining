// import { ComponentFixture, TestBed } from '@angular/core/testing';

// import { Recipe } from './recipe';

// describe('Recipe', () => {
//   let component: Recipe;
//   let fixture: ComponentFixture<Recipe>;

//   beforeEach(async () => {
//     await TestBed.configureTestingModule({
//       imports: [Recipe]
//     })
//     .compileComponents();

//     fixture = TestBed.createComponent(Recipe);
//     component = fixture.componentInstance;
//     fixture.detectChanges();
//   });

//   it('should create', () => {
//     expect(component).toBeTruthy();
//   });
// });

import { ComponentFixture, TestBed } from '@angular/core/testing';
import { Recipe } from './recipe';
import { RecipeModel } from '../models/recipe';
import { By } from '@angular/platform-browser';
import { Component } from '@angular/core';

describe('Recipe', () => {
  let component: Recipe;
  let fixture: ComponentFixture<Recipe>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [Recipe]
    }).compileComponents();

    fixture = TestBed.createComponent(Recipe);
    component = fixture.componentInstance;
  });

  it('should display recipe name', () => {
    const testRecipe: RecipeModel = {
      id: 1,
      name: 'Burger',
      cuisine: 'American',
      cookTimeMinutes: 15,
      ingredients: ['Bun', 'Patty'],
      image: ''
    };

    component.recipe = testRecipe;
    fixture.detectChanges();

    const compiled = fixture.nativeElement as HTMLElement;
    expect(compiled.textContent).toContain('Burger');
    expect(compiled.textContent).toContain('American');
  });
});
