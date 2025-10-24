Imports System.Collections.Generic
Imports System.Linq

''' <summary>
''' Repository class for managing DragonTreasure data operations.
''' </summary>
Public Class DragonTreasureRepository
    Private Shared ReadOnly _treasures As List(Of DragonTreasure) = InitializeSampleData()

    ''' <summary>
    ''' Initializes the repository with sample data.
    ''' </summary>
    Private Shared Function InitializeSampleData() As List(Of DragonTreasure)
        Return New List(Of DragonTreasure) From {
         New DragonTreasure("Golden Chalice") With {
                .Id = 1,
    .Description = "A magnificent golden chalice encrusted with precious gems, said to have belonged to ancient royalty.",
     .Value = 5000,
     .CoolFactor = 8,
          .IsPublished = True,
        .Owner = "Smaug",
     .PlunderedAt = DateTime.Now.AddDays(-30)
 },
 New DragonTreasure("Ruby Ring of Power") With {
     .Id = 2,
     .Description = "An enchanted ruby ring that glows with an inner fire and grants the wearer increased magical abilities.",
             .Value = 12000,
           .CoolFactor = 10,
            .IsPublished = True,
 .Owner = "Draconius",
     .PlunderedAt = DateTime.Now.AddDays(-15)
      },
            New DragonTreasure("Ancient Sword") With {
     .Id = 3,
           .Description = "A legendary sword forged in dragon fire, its blade never dulls and it hums with ancient power.",
        .Value = 8500,
         .CoolFactor = 9,
      .IsPublished = False,
     .Owner = "Pyrothia",
     .PlunderedAt = DateTime.Now.AddDays(-7)
},
     New DragonTreasure("Crystal Orb") With {
    .Id = 4,
          .Description = "A perfectly spherical crystal that shows visions of distant lands and future events.",
        .Value = 15000,
 .CoolFactor = 10,
        .IsPublished = True,
       .Owner = "Smaug",
      .PlunderedAt = DateTime.Now.AddDays(-45)
   },
       New DragonTreasure("Silver Coins") With {
       .Id = 5,
        .Description = "A collection of rare silver coins from various kingdoms, each bearing unique markings.",
                .Value = 2500,
            .CoolFactor = 5,
      .IsPublished = True,
          .Owner = "Vermithrax",
 .PlunderedAt = DateTime.Now.AddDays(-2)
     }
  }
    End Function

    ''' <summary>
    ''' Gets all treasures.
    ''' </summary>
    Public Function GetAll() As List(Of DragonTreasure)
        Return _treasures.ToList()
    End Function

    ''' <summary>
    ''' Gets published treasures only.
    ''' </summary>
    Public Function GetPublished() As List(Of DragonTreasure)
        Return _treasures.Where(Function(t) t.IsPublished).ToList()
    End Function

    ''' <summary>
    ''' Gets a treasure by its ID.
    ''' </summary>
    Public Function GetById(id As Integer) As DragonTreasure
        Return _treasures.FirstOrDefault(Function(t) t.Id = id)
    End Function

    ''' <summary>
    ''' Gets treasures by owner.
    ''' </summary>
    Public Function GetByOwner(owner As String) As List(Of DragonTreasure)
        Return _treasures.Where(Function(t) t.Owner.ToLower().Contains(owner.ToLower())).ToList()
    End Function

    ''' <summary>
    ''' Searches treasures by name or description.
    ''' </summary>
    Public Function Search(query As String) As List(Of DragonTreasure)
        If String.IsNullOrWhiteSpace(query) Then
            Return GetAll()
        End If

        Dim lowerQuery = query.ToLower()
        Return _treasures.Where(Function(t) (Not String.IsNullOrEmpty(t.Name) AndAlso t.Name.ToLower().Contains(lowerQuery)) OrElse (Not String.IsNullOrEmpty(t.Description) AndAlso t.Description.ToLower().Contains(lowerQuery))).ToList()
    End Function

    ''' <summary>
    ''' Adds a new treasure.
    ''' </summary>
    Public Function Add(treasure As DragonTreasure) As DragonTreasure
        treasure.Id = _treasures.Max(Function(t) t.Id) + 1
        _treasures.Add(treasure)
        Return treasure
    End Function

    ''' <summary>
    ''' Updates an existing treasure.
    ''' </summary>
    Public Function Update(id As Integer, updatedTreasure As DragonTreasure) As DragonTreasure
        Dim existingTreasure = GetById(id)
        If existingTreasure Is Nothing Then
            Return Nothing
        End If

        existingTreasure.Name = updatedTreasure.Name
        existingTreasure.Description = updatedTreasure.Description
        existingTreasure.Value = updatedTreasure.Value
        existingTreasure.CoolFactor = updatedTreasure.CoolFactor
        existingTreasure.IsPublished = updatedTreasure.IsPublished
        existingTreasure.Owner = updatedTreasure.Owner

        Return existingTreasure
    End Function

    ''' <summary>
    ''' Deletes a treasure by ID.
    ''' </summary>
    Public Function Delete(id As Integer) As Boolean
        Dim treasure = GetById(id)
        If treasure Is Nothing Then
            Return False
        End If

        _treasures.Remove(treasure)
        Return True
    End Function

    ''' <summary>
    ''' Gets the total count of treasures.
    ''' </summary>
    Public Function GetCount() As Integer
        Return _treasures.Count
    End Function

    ''' <summary>
    ''' Gets treasures with pagination.
    ''' </summary>
    Public Function GetPage(page As Integer, pageSize As Integer) As List(Of DragonTreasure)
        Dim skip = (page - 1) * pageSize
        Return _treasures.Skip(skip).Take(pageSize).ToList()
    End Function
End Class