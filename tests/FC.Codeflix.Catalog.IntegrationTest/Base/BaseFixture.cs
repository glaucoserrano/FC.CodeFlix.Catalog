﻿using Bogus;

namespace FC.Codeflix.Catalog.IntegrationTest.Base;
public  class BaseFixture
{
    protected Faker Faker { get; set; }
    public BaseFixture() 
        => Faker = new Faker("pt_BR");


}
