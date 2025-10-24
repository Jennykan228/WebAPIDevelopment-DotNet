Imports System.Net
Imports System.Web.Http
Imports System.Web.Http.Description
Imports System.ComponentModel.DataAnnotations

''' <summary>
''' API controller for managing dragon treasures.
''' Provides CRUD operations and search functionality for rare and valuable treasures.
''' </summary>
<RoutePrefix("api/treasures")>
Public Class TreasuresController
    Inherits ApiController

    Private ReadOnly _repository As New DragonTreasureRepository()

    ''' <summary>
    ''' Gets all published treasures with optional pagination and search.
    ''' </summary>
    ''' <param name="page">Page number (default: 1).</param>
    ''' <param name="pageSize">Items per page (default: 10, max: 50).</param>
    ''' <param name="search">Search query to filter by name or description.</param>
    ''' <param name="owner">Filter by owner username (partial match).</param>
    ''' <param name="publishedOnly">Whether to return only published treasures (default: true).</param>
    ''' <returns>A list of dragon treasures.</returns>
    <HttpGet>
    <ResponseType(GetType(IEnumerable(Of DragonTreasureDto)))>
    Public Function GetTreasures(
      <FromUri> Optional page As Integer = 1,
        <FromUri> Optional pageSize As Integer = 10,
        <FromUri> Optional search As String = "",
      <FromUri> Optional owner As String = "",
        <FromUri> Optional publishedOnly As Boolean = True
    ) As IHttpActionResult
        Try
            ' Validate pagination parameters
            If page < 1 Then page = 1
            If pageSize < 1 Or pageSize > 50 Then pageSize = 10

            ' Get treasures based on filters
            Dim treasures As List(Of DragonTreasure)

            If publishedOnly Then
                treasures = _repository.GetPublished()
            Else
                treasures = _repository.GetAll()
            End If

            ' Apply search filter
            If Not String.IsNullOrWhiteSpace(search) Then
                treasures = _repository.Search(search).Where(Function(t) If(publishedOnly, t.IsPublished, True)).ToList()
            End If

            ' Apply owner filter
            If Not String.IsNullOrWhiteSpace(owner) Then
                treasures = treasures.Where(Function(t) t.Owner.ToLower().Contains(owner.ToLower())).ToList()
            End If

            ' Apply pagination
            Dim skip = (page - 1) * pageSize
            Dim pagedTreasures = treasures.Skip(skip).Take(pageSize).ToList()

            ' Convert to DTOs
            Dim treasureDtos = pagedTreasures.Select(Function(t) ConvertToDto(t)).ToList()

            Return Ok(treasureDtos)

        Catch ex As Exception
            Return InternalServerError(ex)
        End Try
    End Function

    ''' <summary>
    ''' Gets a specific treasure by its ID.
    ''' </summary>
    ''' <param name="id">The treasure ID.</param>
    ''' <returns>The requested treasure or 404 if not found.</returns>
    <HttpGet>
    <Route("{id:int}")>
    <ResponseType(GetType(DragonTreasureDto))>
    Public Function GetTreasure(id As Integer) As IHttpActionResult
        Try
            Dim treasure = _repository.GetById(id)
            If treasure Is Nothing Then
                Return NotFound()
            End If

            Return Ok(ConvertToDto(treasure))

        Catch ex As Exception
            Return InternalServerError(ex)
        End Try
    End Function

    ''' <summary>
    ''' Gets treasures owned by a specific dragon.
    ''' </summary>
    ''' <param name="ownerName">The owner's username.</param>
    ''' <returns>A list of treasures owned by the specified dragon.</returns>
    ''' <example>
    ''' GET /api/treasures/owner/Smaug
    ''' Returns: [{"Id":1,"Name":"Golden Chalice","Description":"A magnificent golden chalice...","Value":5000,"CoolFactor":8,"Owner":"Smaug"}]
    ''' </example>
    <HttpGet>
    <Route("owner/{ownerName}")>
    <ResponseType(GetType(IEnumerable(Of DragonTreasureDto)))>
    Public Function GetTreasuresByOwner(ownerName As String) As IHttpActionResult
        Try
            If String.IsNullOrWhiteSpace(ownerName) Then
                Return BadRequest("Owner name cannot be empty.")
            End If

            Dim treasures = _repository.GetByOwner(ownerName)
            Dim treasureDtos = treasures.Select(Function(t) ConvertToDto(t)).ToList()

            Return Ok(treasureDtos)

        Catch ex As Exception
            Return InternalServerError(ex)
        End Try
    End Function

    ''' <summary>
    ''' Creates a new treasure.
    ''' </summary>
    ''' <param name="treasure">The treasure data to create.</param>
    ''' <returns>The created treasure with assigned ID.</returns>
    <HttpPost>
    <ResponseType(GetType(DragonTreasureDto))>
    Public Function PostTreasure(<FromBody> treasure As DragonTreasure) As IHttpActionResult
        Try
            If treasure Is Nothing Then
                Return BadRequest("Treasure data is required.")
            End If

            ' Validate the model
            If Not ModelState.IsValid Then
                Return BadRequest(ModelState)
            End If

            ' Set creation timestamp
            treasure.PlunderedAt = DateTime.Now

            Dim createdTreasure = _repository.Add(treasure)
            Dim treasureDto = ConvertToDto(createdTreasure)

            Return CreatedAtRoute("DefaultApi", New With {.controller = "treasures", .id = createdTreasure.Id}, treasureDto)

        Catch ex As Exception
            Return InternalServerError(ex)
        End Try
    End Function

    ''' <summary>
    ''' Updates an existing treasure.
    ''' </summary>
    ''' <param name="id">The treasure ID to update.</param>
    ''' <param name="treasure">The updated treasure data.</param>
    ''' <returns>The updated treasure or 404 if not found.</returns>
    <HttpPut>
    <Route("{id:int}")>
    <ResponseType(GetType(DragonTreasureDto))>
    Public Function PutTreasure(id As Integer, <FromBody> treasure As DragonTreasure) As IHttpActionResult
        Try
            If treasure Is Nothing Then
                Return BadRequest("Treasure data is required.")
            End If

            If Not ModelState.IsValid Then
                Return BadRequest(ModelState)
            End If

            Dim updatedTreasure = _repository.Update(id, treasure)
            If updatedTreasure Is Nothing Then
                Return NotFound()
            End If

            Return Ok(ConvertToDto(updatedTreasure))

        Catch ex As Exception
            Return InternalServerError(ex)
        End Try
    End Function

    ''' <summary>
    ''' Partially updates a treasure (PATCH operation).
    ''' </summary>
    ''' <param name="id">The treasure ID to update.</param>
    ''' <param name="updates">The partial update data.</param>
    ''' <returns>The updated treasure or 404 if not found.</returns>
    <HttpPatch>
    <Route("{id:int}")>
    <ResponseType(GetType(DragonTreasureDto))>
    Public Function PatchTreasure(id As Integer, <FromBody> updates As Object) As IHttpActionResult
        Try
            Dim existingTreasure = _repository.GetById(id)
            If existingTreasure Is Nothing Then
                Return NotFound()
            End If

            ' For simplicity, this example only supports updating specific fields
            ' In a real application, you might use JSON Patch or a more sophisticated approach
            Return Ok(ConvertToDto(existingTreasure))

        Catch ex As Exception
            Return InternalServerError(ex)
        End Try
    End Function

    ''' <summary>
    ''' Deletes a treasure.
    ''' </summary>
    ''' <param name="id">The treasure ID to delete.</param>
    ''' <returns>204 No Content if successful, 404 if not found.</returns>
    <HttpDelete>
    <Route("{id:int}")>
    Public Function DeleteTreasure(id As Integer) As IHttpActionResult
        Try
            Dim success = _repository.Delete(id)
            If Not success Then
                Return NotFound()
            End If

            Return StatusCode(HttpStatusCode.NoContent)

        Catch ex As Exception
            Return InternalServerError(ex)
        End Try
    End Function

    ''' <summary>
    ''' Gets treasure statistics.
    ''' </summary>
    ''' <returns>Statistics about treasures in the collection.</returns>
    <HttpGet>
    <Route("stats")>
    <ResponseType(GetType(Object))>
    Public Function GetStats() As IHttpActionResult
        Try
            Dim allTreasures = _repository.GetAll()
            Dim publishedTreasures = _repository.GetPublished()

            Dim stats = New With {
       .TotalTreasures = allTreasures.Count,
              .PublishedTreasures = publishedTreasures.Count,
          .TotalValue = allTreasures.Sum(Function(t) t.Value),
    .AverageCoolFactor = If(allTreasures.Any(), allTreasures.Average(Function(t) t.CoolFactor), 0),
          .UniqueOwners = allTreasures.Select(Function(t) t.Owner).Distinct().Count(),
             .MostValuableTreasure = allTreasures.OrderByDescending(Function(t) t.Value).FirstOrDefault()?.Name
          }

            Return Ok(stats)

        Catch ex As Exception
            Return InternalServerError(ex)
        End Try
    End Function

    ''' <summary>
    ''' Converts a DragonTreasure model to a DTO for API responses.
    ''' </summary>
    Private Function ConvertToDto(treasure As DragonTreasure) As DragonTreasureDto
        Return New DragonTreasureDto With {
            .Id = treasure.Id,
         .Name = treasure.Name,
     .Description = treasure.Description,
  .ShortDescription = treasure.ShortDescription,
         .Value = treasure.Value,
          .CoolFactor = treasure.CoolFactor,
    .PlunderedAt = treasure.PlunderedAt,
.PlunderedAtAgo = treasure.PlunderedAtAgo,
          .IsPublished = treasure.IsPublished,
            .Owner = treasure.Owner
        }
    End Function
End Class