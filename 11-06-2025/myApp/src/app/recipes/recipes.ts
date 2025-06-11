import { Component, Signal, computed, effect, inject, signal } from '@angular/core';
import { RecipeService } from '../services/recipe.service';
import { RecipeModel } from '../models/recipe';
import { Recipe } from '../recipe/recipe';

@Component({
  selector: 'app-recipes',
  standalone: true,
  imports: [Recipe],
  templateUrl: './recipes.html',
  styleUrl: './recipes.css'
})
export class Recipes {
  private recipeService = inject(RecipeService);

  recipes = signal<RecipeModel[]>([]);

  constructor() {
    this.recipeService.getAllRecipes().subscribe({
      next: (data: any) => {
        this.recipes.set(data.recipes);
      },
      error: () => {
        this.recipes.set([]);
      }
    });
  }
}
