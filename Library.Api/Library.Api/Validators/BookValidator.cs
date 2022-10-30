namespace Library.Api.Validators;

using FluentValidation;
using Library.Api.Models;

public class BookValidator : AbstractValidator <Book>
{
	public BookValidator()
	{
		RuleFor(c => c.Isbn).Matches("^(?=(?:\\D*\\d){10}(?:(?:\\D*\\d){3})?$)[\\d-]+$");
		RuleFor(c => c.Title).NotEmpty();
		RuleFor(c => c.Author).NotEmpty();
		RuleFor(c => c.ShortDescription).NotEmpty();
		RuleFor(c => c.PageCount).GreaterThan(0);
	}
}
