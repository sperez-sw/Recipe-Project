import { TestBed } from '@angular/core/testing';

import { RecipesResolverResolver } from './recipes-resolver.resolver';

describe('RecipesResolverResolver', () => {
  let resolver: RecipesResolverResolver;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    resolver = TestBed.inject(RecipesResolverResolver);
  });

  it('should be created', () => {
    expect(resolver).toBeTruthy();
  });
});
