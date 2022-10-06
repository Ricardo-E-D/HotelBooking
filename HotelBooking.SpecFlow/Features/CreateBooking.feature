Feature: CreateBooking

@mytag
Scenario: Try Create Booking(BB)
	Given the first date is 9
	And the second date is 9
	When a booking is created
	Then the result should be true

Scenario: Try Create Booking(AA)
	Given the first date is 21
	And the second date is 21
	When a booking is created
	Then the result should be true

Scenario: Try Create Booking(BA)
	Given the first date is 9
	And the second date is 21
	When a booking is created
	Then the result should be false

Scenario: Try Create Booking(BO Scenario 1)
	Given the first date is 9
	And the second date is 10
	When a booking is created
	Then the result should be false

Scenario: Try Create Booking(BO Scenario 2)
	Given the first date is 9
	And the second date is 20
	When a booking is created
	Then the result should be false

Scenario: Try Create Booking(OA Scenario 1)
	Given the first date is 10
	And the second date is 21
	When a booking is created
	Then the result should be false

Scenario: Try Create Booking(OA Scenario 2)
	Given the first date is 20
	And the second date is 21
	When a booking is created
	Then the result should be false

Scenario: Try Create Booking(OO Scenario 1)
	Given the first date is 10
	And the second date is 10
	When a booking is created
	Then the result should be false

Scenario: Try Create Booking(OO Scenario 2)
	Given the first date is 10
	And the second date is 20
	When a booking is created
	Then the result should be false

Scenario: Try Create Booking(OO Scenario 3)
	Given the first date is 20
	And the second date is 20
	When a booking is created
	Then the result should be false