// import { ComponentFixture, TestBed } from '@angular/core/testing';

// import { Recipes } from './recipes';

// describe('Recipes', () => {
//   let component: Recipes;
//   let fixture: ComponentFixture<Recipes>;

//   beforeEach(async () => {
//     await TestBed.configureTestingModule({
//       imports: [Recipes]
//     })
//     .compileComponents();

//     fixture = TestBed.createComponent(Recipes);
//     component = fixture.componentInstance;
//     fixture.detectChanges();
//   });

//   it('should create', () => {
//     expect(component).toBeTruthy();
//   });
// });


import { ComponentFixture, TestBed } from '@angular/core/testing';
import { Recipes } from './recipes';
import { RecipeService } from '../services/recipe.service';
import { of, throwError } from 'rxjs';
import { Recipe } from '../recipe/recipe';

describe('Recipes', () => {
  let fixture: ComponentFixture<Recipes>;
  let component: Recipes;
  let recipeServiceSpy: jasmine.SpyObj<RecipeService>;

  const mockRecipes = [
    { id: 1, name: 'Pizza', cuisine: 'Italian', cookTimeMinutes: 30, ingredients: ['Dough'], image: '' }
  ];

  beforeEach(async () => {
    const spy = jasmine.createSpyObj('RecipeService', ['getAllRecipes']);

    await TestBed.configureTestingModule({
      imports: [Recipes, Recipe],
      providers: [{ provide: RecipeService, useValue: spy }]
    }).compileComponents();

    recipeServiceSpy = TestBed.inject(RecipeService) as jasmine.SpyObj<RecipeService>;
  });

  it('should fetch and set recipes on success', () => {
    recipeServiceSpy.getAllRecipes.and.returnValue(of({ recipes: mockRecipes }));
    fixture = TestBed.createComponent(Recipes);
    component = fixture.componentInstance;
    fixture.detectChanges();

    expect(component.recipes()).toEqual(mockRecipes);
  });

  it('should set empty array on error', () => {
    recipeServiceSpy.getAllRecipes.and.returnValue(throwError(() => new Error('Failed')));
    fixture = TestBed.createComponent(Recipes);
    component = fixture.componentInstance;
    fixture.detectChanges();

    expect(component.recipes()).toEqual([]);
  });
});
